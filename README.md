# NetLocator - Microservices IP Geolocation System

A comprehensive microservices architecture for IP address geolocation with caching, batch processing, and concurrency control.

## Architecture Overview

NetLocator consists of three independent microservices that work together to provide efficient IP geolocation services:

```
┌─────────────────────┐    ┌─────────────────────┐    ┌─────────────────────┐
│   IP Lookup Service │    │ IP Detail Cache     │    │ Batch Processing    │
│   (Port 5163)       │    │ Service             │    │ Service             │
│                     │    │ (Port 5169)         │    │ (Port 5002)         │
│ • External API      │    │ • Memory Cache      │    │ • Batch Processing  │
│ • IP Geolocation    │    │ • Concurrency Ctrl  │    │ • Async Processing  │
│ • Error Handling    │    │ • 1min Expiration   │    │ • Status Tracking   │
└─────────────────────┘    └─────────────────────┘    └─────────────────────┘
```

## Quick Start

### Prerequisites
- .NET 9.0 SDK
- Docker & Docker Compose
- Git

### Running with Docker Compose
```bash
# Clone the repository
git clone <repository-url>
cd net-locator

# Start all services
docker compose up --build

# Access services
# IP Lookup: http://localhost:5163/swagger
# IP Detail Cache: http://localhost:5169/swagger
# Batch Processing: http://localhost:5002/swagger
```

### Running Locally
```bash
# Start IP Lookup Service
cd NetLocator.IPLookupService
dotnet run --project NetLocator.IPLookupService.API

# Start IP Detail Cache Service (in new terminal)
cd NetLocator.IPDetailCacheService
dotnet run --project NetLocator.IPDetailCacheService.API

# Start Batch Processing Service (in new terminal)
cd NetLocator.BatchProcessingService
dotnet run --project NetLocator.BatchProcessingService.API
```

## 📋 Service Specifications

### 1. IP Lookup Microservice

**Purpose**: Provides IP geolocation data by integrating with external IP APIs.

#### Features
- **External API Integration**: Integrates with IPStack API for real-time IP geolocation
- **Error Handling**: Custom `IPServiceNotAvailableException` for API communication issues
- **Consistent Data Structure**: Standardized IP detail response format
- **RESTful API**: Clean HTTP endpoints with proper status codes

#### API Endpoints
```
GET /ip/{ipAddress} - Retrieve IP geolocation details
```

#### Response Format
```json
{
  "ip": "8.8.8.8",
  "type": "ipv4",
  "continentName": "North America",
  "countryName": "United States",
  "regionName": "California",
  "city": "Mountain View",
  "zip": "94043",
  "latitude": 37.386,
  "longitude": -122.0838
}
```

#### Error Handling
- **IPServiceNotAvailableException**: When external API is unavailable
- **IpAddressInvalidFormatException**: For invalid IP address formats
- **HTTP Status Codes**: 400 (Bad Request), 500 (Internal Server Error)

### 2. IP Detail Cache Microservice

**Purpose**: Caches IP lookup results to minimize external API calls and improve response times.

#### Features
- **Memory Cache**: .NET MemoryCache implementation with 1-minute expiration
- **Concurrency Control**: Advanced semaphore-based locking per IP address
- **Cache-First Strategy**: Checks cache before external API calls
- **Thread-Safe Operations**: Prevents duplicate API calls for the same IP

#### Concurrency Control Implementation

The cache service implements concurrency control using `SemaphoreSlim`.

#### Benefits of Concurrency Control
- **Prevents Duplicate Calls**: Multiple threads requesting the same IP won't make duplicate external API calls
- **Concurrent Processing**: Different IP addresses can be processed simultaneously
- **Memory Efficient**: Automatic cleanup of unused semaphores
- **Improved Performance**: Waiting threads get cached results instead of making new calls

#### API Endpoints
```
GET /ip/{ipAddress} - Get cached IP details (with concurrency control)
```

### 3. Batch Processing Microservice

**Purpose**: Handles batch IP processing with asynchronous operations and progress tracking.

#### Features
- **Batch Processing**: Accepts arrays of IP addresses for bulk processing
- **GUID Assignment**: Unique batch identifiers for tracking
- **Chunk Processing**: Processes IPs in chunks of 10 for optimal performance
- **Asynchronous Processing**: Non-blocking batch operations
- **Status Tracking**: Real-time progress monitoring
- **High Load Handling**: Designed for concurrent batch operations

#### API Endpoints

##### Create Batch
```http
POST /ip/batch
Content-Type: application/json

{
  "ipAddresses": ["8.8.8.8", "1.1.1.1", "192.168.1.1"]
}
```

**Response:**
```json
{
  "batchId": "12345678-1234-1234-1234-123456789012",
  "totalIpAddresses": 3,
  "createdAt": "2024-01-01T00:00:00Z"
}
```

##### Get Batch Status
```http
GET /ip/batch/{batchId}
```

**Response:**
```json
{
  "batchId": "12345678-1234-1234-1234-123456789012",
  "status": "Processing",
  "totalIpAddresses": 3,
  "processedIpAddresses": 2,
  "successfulIpAddresses": 2,
  "failedIpAddresses": 0,
  "createdAt": "2024-01-01T00:00:00Z",
  "completedAt": null,
  "errors": []
}
```

#### Batch Status Values
- **Pending**: Batch created, not yet started
- **Processing**: Currently processing IP addresses
- **Completed**: All IPs processed successfully
- **Failed**: Processing failed with errors

#### Processing Flow
1. **Batch Creation**: Assign GUID and return immediately
2. **Asynchronous Processing**: Start processing in background
3. **Chunk Processing**: Process IPs in groups of 10
4. **External API Calls**: Retrieve details from IP Lookup service
5. **Cache Storage**: Store results in IP Detail Cache service
6. **Progress Tracking**: Update status in real-time
7. **Completion**: Mark batch as completed

## 🔧 Technical Implementation

### Architecture Patterns
- **3-layer Architecture**: Separation of concerns with API, Business, and Shared layers
- **Dependency Injection**: Proper IoC container configuration
- **Repository Pattern**: Abstracted data access

### Communication & Integration
- **RESTful APIs**: HTTP-based service communication
- **Service Discovery**: Docker network-based service resolution
- **Error Handling**: Comprehensive exception handling with custom exceptions
- **Retry Logic**: Built-in resilience patterns
- **Rate Limiting**: Configurable request throttling

### Caching Strategy
- **Cache-First**: Always check cache before external calls
- **Expiration Policy**: 1-minute TTL for IP details
- **Memory Management**: Automatic cleanup of expired entries
- **Concurrency**: Thread-safe cache operations

### Docker Support
- **Multi-stage Builds**: Optimized container images
- **Health Checks**: Automatic service monitoring
- **Service Dependencies**: Proper startup ordering
- **Network Isolation**: Secure inter-service communication

## 📊 Performance Characteristics

### Batch Processing Performance
- **Chunk Size**: 10 IPs per chunk for optimal performance
- **Parallel Processing**: Multiple chunks processed simultaneously
- **Progress Tracking**: Real-time status updates
- **Error Handling**: Individual IP failure doesn't stop batch

## 🐳 Docker Deployment

### Service Configuration
```yaml
# IP Lookup Service
ip-lookup-service:
  ports: ["5163:8080", "7079:8081"]
  environment:
    - ASPNETCORE_ENVIRONMENT=Development

# IP Detail Cache Service  
ip-detail-cache-service:
  ports: ["5169:8080", "7202:8081"]
  environment:
    - IpLookup__ConnectionString=http://ip-lookup-service:8080
  depends_on: [ip-lookup-service]

# Batch Processing Service
batch-processing-service:
  ports: ["5002:80", "7002:443"]
  environment:
    - BatchProcessing__IpLookupServiceUrl=http://ip-lookup-service:8080
  depends_on: [ip-lookup-service]
```

### Health Checks
All services include comprehensive health monitoring:
- **Endpoint**: `/health`
- **Interval**: 30 seconds
- **Timeout**: 10 seconds
- **Retries**: 3 attempts
- **Start Period**: 40 seconds

### Health Monitoring
- **Service Health**: Individual service status
- **Dependency Health**: External service availability
- **Cache Performance**: Hit/miss ratios
- **Batch Progress**: Real-time processing status

## 🚀 Usage Examples

### Single IP Lookup
```bash
curl -X GET "http://localhost:5169/ip/8.8.8.8"
```

### Batch Processing
```bash
# Create batch
curl -X POST "http://localhost:5002/ip/batch" \
  -H "Content-Type: application/json" \
  -d '{"ipAddresses": ["8.8.8.8", "1.1.1.1", "192.168.1.1"]}'

# Check status
curl -X GET "http://localhost:5002/ip/batch/{batchId}"
```

### Swagger Documentation
- **IP Lookup Service**: http://localhost:5163/swagger
- **IP Detail Cache Service**: http://localhost:5169/swagger
- **Batch Processing Service**: http://localhost:5002/swagger

---

**NetLocator** - Efficient IP Geolocation with Advanced Caching and Batch Processing