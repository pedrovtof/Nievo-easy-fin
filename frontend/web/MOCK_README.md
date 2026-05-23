# Mock Data Guide

This frontend uses a mock data setup to allow development and testing without needing the backend running.

## How to Enable Mock Data

To use the mock data instead of real API calls, you must set an environment variable before running the frontend.

Create a `.env` file in `frontend/web` (if you don't have one) and add the following line:

```
VITE_USE_MOCK=true
```

Then run the development server:

```
npm run dev
```

## How It Works

When `VITE_USE_MOCK` is true, the `src/services/api.js` file uses Axios Interceptors to intercept all outgoing API requests. It then matches the requests to the predefined mock data in `src/services/mockData.js` and returns a mock response without sending the request to the network.

## Mock Guide Popup

When running in Mock mode, you will see a small Floating Action Button (FAB) in the bottom right corner of the screen. 
Clicking this button opens a popup that explains you are in mock mode. It provides quick access to copy the mock data (e.g., standard login credentials or response structures) to your clipboard for easy testing.

## Modifying Mock Data

If you need to change the mock data responses (e.g., to simulate an error or different data sets), edit the `frontend/web/src/services/mockData.js` file.
