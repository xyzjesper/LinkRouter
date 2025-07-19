# 📡 LinkRouter

**LinkRouter** is a lightweight routing application designed to route incoming requests to specific URLs based on a configuration file. It primarily handles links from the `.link` domain and provides flexible routing for custom links such as `/instagram`, `/youtube/test`, or any other custom path.

## ✨ Features

- 🔗 Routes requests based on a pre-configured set of paths.
- 🚪 Handles both trailing and non-trailing slashes.
- 🌍 Supports external redirects (e.g., to social media profiles, websites, etc.).
- ⚙️ Configuration-driven, allowing easy updates of routing paths.
- 📜 Logs requests and routes for monitoring and debugging purposes.

## 🛠 Prerequisites

- A **domain** (optional)
- **Docker** or **Docker Compose** installed for containerized deployment.

## 🔧 Configuration

Routes are managed via a configuration file, `/data/config.json`. You can define paths and their corresponding URLs in this file. The application automatically normalizes routes to handle both trailing and non-trailing slashes.

```json
{
  "RootRoute": "https://example.com", // route on the root on the app (eg: yourdomain.com)
  "Routes": [
    {
      "Route": "/instagram", // has to start with a slash
      "RedirectUrl": "https://instagram.com/{yourname}"
    },
    {
      "Route": "/example", // has to start with a slash
      "RedirectUrl": "https://example.com"
    }
  ]
}
```

## 💻 Usage

Once the application is running, it will listen for requests and route them based on the paths defined in your configuration file.

## 🐳 Docker Deployment

### Docker Compose

LinkRouter includes a `docker-compose.yml` file for easy containerized deployment. With Docker Compose, you can quickly spin up the application using predefined settings.

### Docker Compose Example:

Create a `docker-compose.yml` file with the following content:

```yaml
services:
  linkrouter:
    image: ghcr.io/mxritzdev/linkrouter:latest
    ports:
      - "80:8080"
    volumes:
      - ./data:/app/data
```

### Running the Application with Docker Compose:

1. Start the container with Docker Compose:

   ```bash
   docker-compose up -d
   ```

This will start the application and map port `80` on your local machine to port `80` inside the container.

### Docker Run Command

Alternatively, you can run the application directly with a `docker run` command if you already have the image.

```bash
docker run -d -p 80:8080 -v ./data:/app/data ghcr.io/mxritzdev/linkrouter:latest
```

This command runs the container in detached mode, binds port `80` on your local machine to port `80` in the container, and mounts the `/data` folder so the container can use your configuration.

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🤝 Contributing

We welcome contributions! Please submit a pull request or open an issue to discuss improvements or new features.

## 📬 Contact

For questions or support, please reach out via discord at **mxritzdev**
