# HiHoHu - Modern Blogging Platform

[![Build Status](https://github.com/caovanhoang63/HiHoHuBlog/workflows/ci/badge.svg)](https://github.com/caovanhoang63/HiHoHuBlog/actions)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)


Hihohu is a modern blogging platform built with Blazor Server Side, inspired by Medium's user experience. It provides a seamless writing and reading experience with powerful search capabilities and robust cloud infrastructure.


## Members
- **[Cao Văn Hoàng](https://github.com/caovanhoang63)**
- **[Bùi Trọng Hoàng Huy](https://github.com/BuiTrongHoangHuy)**
- **[Nguyễn Huỳnh Duy Hiếu](https://github.com/nhdhieuu)**

## 🔗 Demo

- **Production Site**: [https://wwww.hihohu.site](https://www.hihohu.site)

## 🚀 Features

- Real-time collaborative editing
- Advanced search functionality powered by OpenSearch
- Responsive design with Tailwind CSS and Daisy UI
- Email notifications using AWS SES
- Social features (follows, likes, comments)
- Tag-based content organization
- User authentication and authorization
- Reading time estimation
- Bookmarks and reading lists

## 🛠 Technology Stack

- **Frontend & Backend**
    - Blazor Server Side (.NET 8)
    - Tailwind CSS
    - Daisy UI

- **Database & Search**
    - MySQL (AWS RDS)
    - OpenSearch (AWS)

- **Cloud Infrastructure**
    - AWS EC2 (Application Hosting)
    - AWS RDS (Database)
    - AWS OpenSearch (Search Engine)
    - AWS SES (Email Service)
    - AWS S3 (Media Storage)

- **DevOps**
    - GitHub Actions (CI/CD)
    - Docker
    - Nginx
  - **DNS** 
    - Cloudflare

## DEV
```bash 
npx tailwindcss -i wwwroot/app.css -o wwwroot/app.min.css --watch

dotnet watch
```

## 🏗 Architecture
- Clean architecture

## 🚦 Prerequisites

- .NET 8 SDK
- MySQL
- Node.js (for Tailwind CSS)
- Docker & Docker Compose
- AWS CLI configured with appropriate permissions

## 🛠 Local Development Setup

1. Clone the repository:
```bash
git clone https://github.com/yourusername/hiholive.git
cd hiholive
```

2. Install dependencies:
```bash
dotnet restore
npm install
```

3. Start local services:
```bash
docker-compose up -d
```
4. Start the application:
```bash
dotnet watch 
```

## 🚀 Deployment

### Using GitHub Actions

The project includes GitHub Actions workflows for:
- Continuous Integration (Build & Test)
- Continuous Deployment to AWS EC2
- Infrastructure updates via Terraform

Deployment steps are automated through `.github/workflows/deploy.yml`.

### Manual Deployment

1. Build the application:
```bash
dotnet publish -c Release
```

## 🔧 Configuration

Key configuration areas:

- `appsettings.json`: Application settings

## 📝 Contributing

1. Fork the repository
2. Create a feature branch
3. Commit your changes
4. Push to the branch
5. Create a Pull Request

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🤝 Support

For support, email caovanhoang204@gmail.com or open an issue in the GitHub repository.

## 🙏 Acknowledgments

- Blazor Team at Microsoft
- Tailwind CSS & Daisy UI Communities
- AWS for cloud infrastructure
- All contributors and supporters
- Demo blogs from [Medium](https://medium.com), [Viblo](https://viblo.asia), ...


## Ref
- Link Figma: https://www.figma.com/design/PhxS2VbYVg5enKW5Q01R7z/Untitled?m=auto&t=m9kIkEZbdCCcvBsT-6
- WYSIWYG Repo https://github.com/somegenericdev/WYSIWYGTextEditor

