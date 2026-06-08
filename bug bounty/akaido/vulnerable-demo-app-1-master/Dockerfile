FROM node:18 AS builder

WORKDIR /build



# --------------------------------------------------

FROM node:18-slim

WORKDIR /usr/src/app



COPY package*.json ./
RUN npm ci

COPY . .

USER node
EXPOSE 3000

CMD ["node", "src/index.js"]
