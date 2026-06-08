resource "aws_route53_zone" "public" {
  name    = "demo-app.example"
  comment = "Public hosted zone for the demo app"

  tags = {
    App         = "vulnerable-demo-app-1"
    Environment = "demo"
  }
}

resource "aws_route53_hosted_zone_dnssec" "public" {
  hosted_zone_id = aws_route53_zone.public.id
}

resource "aws_route53_record" "app_a" {
  zone_id = aws_route53_zone.public.zone_id
  name    = "app.${aws_route53_zone.public.name}"
  type    = "A"
  ttl     = 300
  # TEST-NET-3 placeholder IP (non-routable, safe for examples)
  records = ["203.0.113.10"]
}
