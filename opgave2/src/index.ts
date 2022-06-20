import { UserModel } from "./models/UserModel";
import http from "http";
import { Express } from "express";
import { EntityManager, EntityRepository, MikroORM } from "@mikro-orm/core";
import { BetterSqliteDriver } from "@mikro-orm/better-sqlite";
import { merge } from "lodash";
import RootResolver from "./resolvers/RootResolver";
import UserResolvers from "./resolvers/UserResolvers";
import startApolloServer from "./app";
import typeDefs from "./schema";

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

  const resolvers = merge(RootResolver, UserResolvers);

  await startApolloServer(typeDefs, resolvers);
};

bootstrap();
