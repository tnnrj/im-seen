# IM Foundation App

## Deployment
A Dockerfile has been included to ease deployment to Docker platforms on any host. Acting in the 
appropriate directory, with Node installed, first run `npm install` to add necessary dependencies.
Then, construct the Docker image:
`docker build -t im-stack .`
And run the app on port 80:
`docker run -d --name im-stack -p 80:80 im-stack`