import { simpleWebServer } from "./simple-web-server";
import { UserModel } from "./models/UserModel";
import http from "http";
import { Express } from "express";
import { EntityManager, EntityRepository, MikroORM } from "@mikro-orm/core";
import { BetterSqliteDriver } from "@mikro-orm/better-sqlite";
import server from "./app";

export const DI = {} as {
  server: http.Server;
  orm: MikroORM;
  em: EntityManager;
  userRepository: EntityRepository<UserModel>;
};

const bootstrap = async () => {
  DI.orm = await MikroORM.init<BetterSqliteDriver>({
    entities: [__dirname + "/models/**/*.ts"],
    dbName: "mikro-orm-sqlite",
    type: "better-sqlite", //better performance SQLite
    debug: true,
    allowGlobalContext: true,
  });

  await DI.orm.getSchemaGenerator().updateSchema();

  DI.em = DI.orm.em;
  DI.userRepository = DI.em.getRepository(UserModel);

  simpleWebServer.listen(3334, () => {
    console.log("Dumb server listening on port 3334 🥴");
  });

  DI.server = server.listen(3333, () => {
    console.log("Listening on port 3333 🚀");
  });
};

bootstrap();
