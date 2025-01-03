# ExchangeRates API

ExchangeRates API is a .NET 8 application designed to manage and fetch foreign exchange rates. It uses a clean architecture approach (DDD - Domain-Driven Design) and provides integration with external APIs for real-time exchange rate updates.

# 🌟 Features

- Domain-Driven Design (DDD): Cleanly separated application layers.
- External API Integration: Fetches live exchange rates using Alpha Vantage.
- RESTful API: Exposes endpoints for adding, updating, and retrieving exchange rates.
- Swagger UI: Interactive documentation available.
- Docker Support: Easily deployable using Docker Compose.
- PostgreSQL Database: Stores exchange rate data.
- Unit Tests: Comprehensive tests for critical functionalities.

---

# 📂 Project Structure

```
├── Application    # Business logic and application services
├── Domain         # Core domain entities and logic
├── Infrastructure # Data access and external API integrations
├── Presentation   # REST API controllers
├── Tests          # Unit tests
└── README.md      # Project documentation
```

# 🛠️ Prerequisites
```
# Install .NET 8 SDK
# Download and install from https://dotnet.microsoft.com/download

# Install Docker
# Download and install from https://www.docker.com/products/docker-desktop

# Install Visual Studio 2022 (Optional)
# Download from https://visualstudio.microsoft.com/
```

# 🚀 Getting Started

Running Locally with Visual Studio

```
# Clone the Repository
git clone https://github.com/your-repository/exchange-rates.git
cd exchange-rates
```
## Configure Environment Variables  
```
To use the Alpha Vantage API, you'll need to generate an API key. Here's how to do it:  

1. Visit [Alpha Vantage](https://www.alphavantage.co).  
2. Sign up for a free account or log in if you already have one.  
3. Navigate to your profile or account settings to generate a new API key.  
4. Copy the API key provided to you.  

You will need to configure this API key in your application. Instead of directly updating appsettings.json, we recommend setting it up as an environment variable or using a configuration method that fits your application's deployment strategy.  

For example, you might set the key as an environment variable like this:  
- On Linux/MacOS: export ALPHA_VANTAGE_API_KEY=your-api-key 
- On Windows (Command Prompt): set ALPHA_VANTAGE_API_KEY=your-api-key 

Then, use your code or framework to read the environment variable dynamically.  

We recommend using the CURRENCY_EXCHANGE_RATE function of [Alpha Vantage](https://www.alphavantage.co).

# Start the Application
# Open the project in Visual Studio.
# Set Presentation as the startup project.
# Press F5 to run the application.

# Access Swagger
# Navigate to http://localhost:8080/swagger/index.html
```
Running with Docker Compose
```
# Clone the Repository
git clone https://github.com/your-repository/exchange-rates.git
cd exchange-rates

# Start the Services
docker-compose up --build

# Access Swagger
# Navigate to http://localhost:8080/swagger/index.html

# Verify Services
# PostgreSQL: Runs on port 5432
# PGAdmin: Access at http://localhost:5050
# Email: admin@admin.com
# Password: admin
```

# 🧪 Testing

```
# Run Unit Tests
dotnet test

# Test Endpoints
# Use Swagger UI at http://localhost:8080/swagger/index.html or tools like Postman.
```

# 🛡️ Security Considerations
```
# Secrets Management
# For local development, use .env or Docker secrets.
# Never commit secrets like API keys or database credentials to version control.

# HTTPS
# For production, ensure HTTPS is enabled.
```

# 🖥️ API Endpoints
```
# Method | Endpoint                                      | Description
# -------|-----------------------------------------------|------------------------------------
# GET    | /api/rates/{baseCurrency}/{quoteCurrency}     | Retrieve a specific exchange rate
# POST   | /api/rates                                    | Add or update an exchange rate

# For more details, refer to the Swagger UI at http://localhost:8080/swagger/index.html
```

# 🛠️ Built With
```
# .NET 8: Modern web development.
# Entity Framework Core: Data access and migrations.
# PostgreSQL: Database backend.
# Docker Compose: Container orchestration.
# Alpha Vantage API: External exchange rate provider.
```

# 📜 License
```
# This project is licensed under the MIT License - see the LICENSE file for details.
```

# 🤝 Contributing
```
# Fork the repository
git fork https://github.com/your-repository/exchange-rates.git

# Create a feature branch
git checkout -b feature-name

# Commit your changes
git commit -m "Add some feature"

# Push to the branch
git push origin feature-name

# Open a pull request
# Submit your pull request on GitHub.
```

# 📧 Contact
```
# Name: Igor Maciel
# Email: igorbmaciel@yahoo.com.br
```
