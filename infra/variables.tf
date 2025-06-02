variable "db_username" {
  description = "Username for RDS"
  type        = string
}

variable "db_password" {
  description = "Password for RDS"
  type        = string
  sensitive   = true
}

variable "db_name" {
  description = "Database name"
  default     = "devopsdb"
}

variable "app_image_tag" {
  description = "ECR Docker image tag to deploy"
  type        = string
}

