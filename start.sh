#!/bin/bash

echo "Starting XML Editor..."
echo

echo "Installing frontend dependencies..."
cd frontend
npm install
if [ $? -ne 0 ]; then
    echo "Failed to install frontend dependencies"
    exit 1
fi

echo
echo "Starting backend server..."
cd ../backend
dotnet run &
BACKEND_PID=$!

echo
echo "Waiting for backend to start..."
sleep 5

echo
echo "Starting frontend..."
cd ../frontend
npm start &
FRONTEND_PID=$!

echo
echo "XML Editor is starting up..."
echo "Backend: http://localhost:5000"
echo "Frontend: http://localhost:3000"
echo
echo "Press Ctrl+C to stop both servers"

# Function to cleanup background processes
cleanup() {
    echo
    echo "Stopping servers..."
    kill $BACKEND_PID 2>/dev/null
    kill $FRONTEND_PID 2>/dev/null
    exit 0
}

# Set trap to cleanup on script exit
trap cleanup SIGINT SIGTERM

# Wait for user to stop
wait
