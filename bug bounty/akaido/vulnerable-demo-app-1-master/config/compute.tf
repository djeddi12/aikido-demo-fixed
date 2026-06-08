resource "aws_iam_role" "app_role" {
  name = "demo-app-ec2-role"

  assume_role_policy = jsonencode({
    Version = "2012-10-17",
    Statement = [{
      Effect = "Allow",
      Principal = { Service = "ec2.amazonaws.com" },
      Action   = "sts:AssumeRole"
    }]
  })
}

resource "aws_iam_instance_profile" "app_profile" {
  name = "demo-app-ec2-profile"
  role = aws_iam_role.app_role.name
}

resource "aws_instance" "app" {
  ami                    = "ami-12345678"     # placeholder
  instance_type          = "t3.micro"
  iam_instance_profile   = aws_iam_instance_profile.app_profile.name

  tags = {
    Name = "vulnerable-demo-app-1"
  }

  metadata_options {
    http_endpoint = "enabled"
    http_tokens   = "required"
  }
}
