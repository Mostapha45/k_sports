# KSports
KSports is a web application that allows users to browse and purchase sports-related products. The application consists of a frontend built with Next.js and a backend built with ASP.NET Core, with Stripe for payments handling.

It has the features of displaying products, search for products, make purchases, and track purchase orders. 

The server is deployed on Azure with Docker container, and frontend is deployed on Vercel.


## Table of Contents
- [Getting started](#getting-started)
- [Installation and Setup](#installation-and-setup)
- [Frontend Code](#frontend)
- [Backend code](#backend-code)

## Getting Started
### Prerequisites
Node.js
npm or yarn
.NET SDK


### Installation and Setup

1. Clone the repository:
```bash
git clone https://github.com/KlintLee115/k_sports.git
cd ksports
```

2. Install frontend dependencies:
```bash
cd frontend
npm install
# or
yarn install
```

3. Install backend dependencies:
```bash
cd ../backend
dotnet restore
```

## Running the Application
### Frontend
1. Start the development server:
```bash
cd frontend
npm run dev
# or
yarn dev
```

2. Open http://localhost:3000 with your browser to see the result.

### Backend
1. Start the backend server:
```bash
cd backend
dotnet run
```

2. The backend server will be running at http://localhost:5204.

## Frontend Code
```
frontend/                                # Root directory for the frontend application
├── public/                              # Contains static files that are served directly
│   └── ...                              # Various static assets (images, icons, etc.)
├── src/                                 # Contains the source code for the frontend application
│   ├── app/                             # Contains the main application components and pages
│   |   ├── (home)/                      # Home page
│   │   ├── api/                         # Contains API route handlers
│   │   │   └── auth/                    # Contains authentication-related API routes
│   │   │       └── [...nextauth]/       # Contains NextAuth.js configuration
│   │   │           └── route.js         # Defines the authentication routes
│   |   ├── checkout/                    # Checkout page
│   |   ├── paymentfailed/               # Payment failed page
│   |   ├── paymentsuccess/              # Payment success page
│   |   ├── checkout/                    # Order tracking page
│   ├── components/                      # Contains reusable UI components
│   │   │   └── ...                      
│   ├── contexts/                        # React contexts with useContext hook
│   │   │   └── ...                      
│   ├── helpers/                         # Contains helper functions and utilities for API calls and other utilities
│   │   │   └── helpers.ts               
│   └── types/                           # Contains custom TypeScript type definitions
│   │       └── types.ts                 
├── .eslintrc.json                       # Configuration file for ESLint
├── .gitignore                           # Specifies files and directories to be ignored by Git
├── next.config.js                       # Configuration file for Next.js
├── package.json                         # Defines the project's dependencies and scripts
├── tailwind.config.ts                   # Configuration file for Tailwind CSS
├── tsconfig.json                        # Configuration file for TypeScript
└── yarn.lock                            # Lockfile for yarn dependencies
```

## Backend Code
```
backend/
├── Controllers/
│   ├── CheckoutController.cs        // Handles checkout-related requests
│   ├── OrderStatusesController.cs   // Check order statuses
│   ├── ProductsController.cs        // Get products based on sorting and filtering options
│   ├── SeedController.cs            // Handles database seeding requests
│   └── WordsController.cs           // Handles word-related requests, finding words related to current selected products
├── Data/
│   ├── DataContext.cs              // Contains DbSets anad ModelCreation configuration
│   └── Seed.cs                     // Contains the logic for seeding the database with initial data
├── Mappers/
│   └── ProductMapper.cs            // Maps Product entities to ProductResponseDto
├── Models/
│   ├── Product.cs                  // Represents a product entity
│   ├── ProductRelatedWord.cs       // Represents a product-related word entity
│   └── ProductResponseDto.cs       // Represents a data transfer object for products
├── Properties/                     // Contains project properties
│   └── launchSettings.json        // Contains settings for launching the application
├── Dockerfile
├── backend.csproj
└── Program.cs
```

## API Endpoints
#### /api/Products
- `GET` /api/products: Get a list of products with optional filters and sorting.

#### /api/Words
- `GET` /api/words: Get data related to words with optional filters.

#### /api/Order Statuses
- `GET` /api/orderstatuses: Get the order status data.

#### /api/Checkout
- `POST` /api/checkout: Create a checkout session.

#### /api/Seed
- `POST` /api/seed: Seed the database with initial data.