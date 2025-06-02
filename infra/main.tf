resource "aws_ecr_repository" "api" {
  name = "infraforge-api"
}

# VPC (simplest option: use default VPC for now)
data "aws_vpc" "default" {
  default = true
}

data "aws_subnets" "default" {
  filter {
    name   = "vpc-id"
    values = [data.aws_vpc.default.id]
  }
}

# ECS Cluster
resource "aws_ecs_cluster" "main" {
  name = "infraforge-cluster"
}

resource "aws_iam_role" "ecs_task_execution" {
  name = "ecsTaskExecutionRole"

  assume_role_policy = jsonencode({
    Version = "2012-10-17"
    Statement = [{
      Action    = "sts:AssumeRole"
      Effect    = "Allow"
      Principal = {
        Service = "ecs-tasks.amazonaws.com"
      }
    }]
  })
}

resource "aws_iam_role_policy_attachment" "ecs_task_execution_policy" {
  role       = aws_iam_role.ecs_task_execution.name
  policy_arn = "arn:aws:iam::aws:policy/service-role/AmazonECSTaskExecutionRolePolicy"
}

resource "aws_cloudwatch_log_group" "ecs_api" {
  name              = "/ecs/infraforge-api"
  retention_in_days = 7
}

resource "aws_ecs_task_definition" "api" {
  family                   = "infraforge-api"
  network_mode             = "awsvpc"
  requires_compatibilities = ["FARGATE"]
  cpu                      = "256"    # 0.25 vCPU
  memory                   = "512"    # 0.5 GB RAM
  execution_role_arn       = aws_iam_role.ecs_task_execution.arn

  container_definitions = jsonencode([
    {
      name      = "api"
      image     = "${aws_ecr_repository.api.repository_url}:latest"
      essential = true
      portMappings = [
        {
          containerPort = 80
          hostPort      = 80
          protocol      = "tcp"
        }
      ]
      environment = [
    {
        name  = "ConnectionStrings__DefaultConnection"
        value = "Host=${aws_db_instance.postgres.endpoint};Port=5432;Database=${var.db_name};Username=${var.db_username};Password=${var.db_password}"
    }
      ]
      logConfiguration = {
        logDriver = "awslogs"
        options = {
          awslogs-group         = "/ecs/infraforge-api"
          awslogs-region        = var.aws_region
          awslogs-stream-prefix = "ecs"
        }
      }
    }
  ])
}

# Security group for the ECS tasks (allow inbound HTTP from ALB)
resource "aws_security_group" "api" {
  name        = "infraforge-api-sg"
  description = "Allow HTTP inbound from ALB"
  vpc_id      = data.aws_vpc.default.id

  ingress {
    from_port   = 80
    to_port     = 80
    protocol    = "tcp"
    cidr_blocks = ["0.0.0.0/0"]  # Open to the internet (for now)
  }

  egress {
    from_port   = 0
    to_port     = 0
    protocol    = "-1"
    cidr_blocks = ["0.0.0.0/0"]
  }
}

# Application Load Balancer (public)
resource "aws_lb" "api" {
  name               = "infraforge-alb"
  internal           = false
  load_balancer_type = "application"
  security_groups    = [aws_security_group.api.id]
  subnets            = data.aws_subnets.default.ids
}

# Target Group (for ALB to route traffic to ECS tasks)
resource "aws_lb_target_group" "api" {
  name        = "infraforge-tg"
  port        = 80
  protocol    = "HTTP"
  vpc_id      = data.aws_vpc.default.id
  target_type = "ip"
  health_check {
    path                = "/"
    interval            = 30
    timeout             = 5
    healthy_threshold   = 2
    unhealthy_threshold = 2
    matcher             = "200"
  }
}

resource "aws_lb_listener" "api" {
  load_balancer_arn = aws_lb.api.arn
  port              = 80
  protocol          = "HTTP"

  default_action {
    type             = "forward"
    target_group_arn = aws_lb_target_group.api.arn
  }
}

resource "aws_ecs_service" "api" {
  name            = "infraforge-api-svc"
  cluster         = aws_ecs_cluster.main.id
  task_definition = aws_ecs_task_definition.api.arn
  desired_count   = 1
  launch_type     = "FARGATE"

  network_configuration {
    subnets          = data.aws_subnets.default.ids
    security_groups  = [aws_security_group.api.id]
    assign_public_ip = true
  }

  load_balancer {
    target_group_arn = aws_lb_target_group.api.arn
    container_name   = "api"
    container_port   = 80
  }

  depends_on = [aws_lb_listener.api]
}

resource "aws_db_instance" "postgres" {
  identifier              = "infraforge-db"
  engine                  = "postgres"
  instance_class          = "db.t3.micro"
  allocated_storage       = 20
  db_name                    = var.db_name
  username                = var.db_username
  password                = var.db_password
  publicly_accessible     = true # For demo/dev only! (Lock down in production)
  skip_final_snapshot     = true
  vpc_security_group_ids  = [aws_security_group.api.id]
}



