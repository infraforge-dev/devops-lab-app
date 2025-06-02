output "alb_dns_name" {
  value = aws_lb.api.dns_name
}

output "db_endpoint" {
  value = aws_db_instance.postgres.endpoint
}
