FROM node:18 AS builder

WORKDIR /build

RUN curl -o important.html https://www.google.com/

# --------------------------------------------------

FROM node:18-slim

WORKDIR /usr/src/app

COPY --from=builder /build/important.html ./important.html

COPY package*.json ./
RUN npm ci

COPY . .

USER node
EXPOSE 3000

CMD ["node", "src/index.js"]
