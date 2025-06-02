provider "aws" {
    region = var.aws_region
}

variable "aws_region" {
    description = "AWS Region for deployment"
    default = "us-west-2"
}
