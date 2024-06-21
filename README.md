**Task Management Application**

**Overview**

The Task Management Application is a web-based tool designed to help users manage their tasks efficiently. The application follows Clean Architecture principles, ensuring a modular, maintainable, and testable codebase. This README file provides instructions on setting up and running the application, along with documentation of any assumptions made during design and implementation.

**Architecture**

Open **"Task Management App Architecture.docx"** file to understand the detailed architecture used to design and develop this project.


**Demo Links**
- UI: [Task Management App UI](https://ad-demo-taskmanagementapp.azurewebsites.net/)
- API: [Task Management App API](https://ad-demo-taskmanagementapp.azurewebsites.net/swagger/index.html)

**Assumptions**
1. **User Authentication:** It is assumed that user authentication is handled using JWT tokens, and the token management (generation, storage, and validation) is implemented securely.
2. **Database:** The application uses an Azure SQL Database, which is set to auto-pause when idle. The application includes a mechanism to wake the database before processing user requests.
3. **Deployment:** The application is deployed on a single Azure App Service that hosts both the Angular frontend and the .NET Core backend API.
4. **Testing:** Unit and integration tests are performed using xUnit and Moq for backend services, and Jasmine/Karma for frontend components.
5. **Environment Configuration:** The application configuration (e.g., database connection strings, API keys) is managed using environment variables.

**Prerequisites**
- .NET 8 SDK
- Node.js (20 LTS version)
- Angular CLI (17 version)
- SQL Server (Azure SQL Database or local SQL Server for development)

**Setup Instructions**
**Backend (ASP.NET Core API)**
  1. Clone the repository:
     
    git clone https://github.com/your-repo/taskmanagementapp.git
    cd taskmanagementapp/TaskManagementApp.WebAPI

  2. Restore dependencies:

    dotnet restore

  3. Update appsettings.json:

    Configure your database connection string in the appsettings.json file:
      
      {
        "ConnectionStrings": {
          "DefaultConnection": "Server=your-server.database.windows.net;Database=your-database;User Id=your-username;Password=your-password;"
        }
      }

  4. Apply migrations:

    dotnet ef database update

  5. Run the application:

    dotnet run

The API will be available at https://localhost:5001/swagger/index.html (for local development).

**Frontend (Angular UI)**
  1. Navigate to the frontend directory:

    cd ../TaskManagementApp.UI

  2. Install dependencies:

    npm install

  3. Update environment.ts:
    Configure the API endpoint in src/environments/environment.ts:
      
      export const environment = {
        production: false,
        apiUrl: 'https://localhost:5001/api'
      };
     
  4. Build the application:

    ng build

  5. Run the application:

    ng serve

The UI will be available at http://localhost:4200 (for local development).

**Deployment**
- Azure App Service: Both the backend and frontend are deployed on a single Azure App Service.
- CI/CD Pipeline: The application uses GitHub Actions for CI/CD. The pipeline automates building, testing, and deploying the application to Azure.

**Contact**
For any questions or issues, please contact _**Kotawala_Adnan@yahoo.co.in**_
