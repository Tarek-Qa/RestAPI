# RESTful API Function App on Azure

## Overview

This RESTful API Function App, deployed on Azure, includes a total of 6 functions. The API provides full CRUD operations for managing products stored in a SQL database hosted on Azure. Additionally, it integrates with the OpenAI service to generate AI responses.

## Key Features

### 1. GetAIResponse
- **Description**: This is a GET function with `AuthorizationLevel.Anonymous` that allows users to interact with OpenAI. You can try it out and chat with the AI by using Postman or directly accessing the endpoint.
- **Usage**:
  - **Postman**: Use the following link, replacing `{promot}` with your question:
    ```
    https://tq-restapi-fa.azurewebsites.net/api/chat/{promot}
    ```
  - **Swagger UI**: If you don't have Postman, you can use Swagger UI to interact with the API:
    ```
    https://tq-restapi-fa.azurewebsites.net/api/swagger/ui
    ```

### 2. CRUD Operations for Product Class
- These functions provide Create, Read, Update, and Delete operations for managing products in the SQL database.
- **Authorization**: All these functions are set to `AuthorizationLevel.Function`, ensuring they are secured with a function key. You will need the function key to execute these operations outside of the Azure environment.
- **Functions**:
  1. **GetAllProducts**
     - **Description**: Retrieves a list of all products.
     - **Endpoint**: `GET /api/products`
  2. **GetProductById**
     - **Description**: Retrieves a product by its ID.
     - **Endpoint**: `GET /api/product/{id}`
  3. **CreateProduct**
     - **Description**: Creates a new product.
     - **Endpoint**: `POST /api/products`
  4. **UpdateProduct**
     - **Description**: Updates an existing product by its ID.
     - **Endpoint**: `PUT /api/product/{id}`
  5. **DeleteProduct**
     - **Description**: Deletes a product by its ID.
     - **Endpoint**: `DELETE /api/product/{id}`

## Security

- The AI response function (`GetAIResponse`) is publicly accessible and can be tested easily.
- The CRUD functions are secured with function-level authorization and require a function key for access outside of Azure.
- This ensures secure operations on the product data.
