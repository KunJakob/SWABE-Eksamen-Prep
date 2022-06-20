import { DI } from './index';
import { RequestContext } from "@mikro-orm/core";
import express, { ErrorRequestHandler } from "express";
import UserController from "./controllers/UserController";

const errorHandler: ErrorRequestHandler = (err, req, res, next) => {
  const { message } = err;
  console.error(message);
  res.status(500).json({ message: "Something went wrong", reason: message });
};

const server = express();
server.use(express.json());


//MikroORM Middleware. New EntityManager context per-request
server.use((req, res, next) => {
    RequestContext.create(DI.em, next);
  });

server.use("/user", UserController);
server.use("/error", () => {
  throw new Error("Unhandled error");
});
server.use("/", (req, res) => {
  res.json({ message: "Hello World" });
});

server.use(errorHandler);

export default server;
