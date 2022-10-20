# MicroservicesDemo

A project created by following Les Jackson's Microservices course.

It is available on YouTube [here](https://www.youtube.com/watch?v=DgVjEo3OGBI).

Some slight modifications have been made to account usage of ASP.NET Core 6.

# Technologies
- ASP.NET Core 6
- Entity Framework Core
- SQL Server
- Docker
- Kubernetes
- RabbitMQ
- gRPC

# Getting starting

## 1. Clone the project

```bash
git clone https://github.com/aywee26/MicroservicesDemo.git
```

## 2. Ingress NGINX

Input the following commands to deploy:

```bash
kubectl apply -f https://raw.githubusercontent.com/kubernetes/ingress-nginx/controller-v1.4.0/deploy/static/provider/cloud/deploy.yaml
```

and apply configuration

```bash
kubectl apply -f .\k8s\ingress-srv.yaml
```

## 3. RabbitMQ

```bash
kubectl apply -f .\k8s\rabbitmq-depl.yaml
```

You can access it here:

```
http://localhost:15672/
```

## 4. Database

Create Kubernetes Secret

```bash
kubectl create secret generic mssql --from-literal=SA_PASSWORD="{your password here}"
```

setup Persistent Volume Claim

```bash
kubectl apply -f .\k8s\local-pvc.yaml
```

and deploy SQL Server

```bash
kubectl apply -f .\k8s\mssql-plat-depl.yaml
```

You can access it via SSMS or Azure Data Studio:

```
Server name: localhost,1433
Authentication: SQL Server Authentication
Login: sa
Password: {your password here}
```

## 5. Deploy PlatformService and CommandService

```bash
kubectl apply -f .\k8s\platforms-depl.yaml
kubectl apply -f .\k8s\commands-depl.yaml
```

You can access them via cURL, Postman or other HTTP client:
```
http://acme.com/api/platforms
http://acme.com/api/c/platforms
```
