import express from "express";
import http from "http";
import { ApolloServer } from "apollo-server-express";
import { ApolloServerPluginDrainHttpServer } from "apollo-server-core";
import { DocumentNode } from "graphql";

async function startApolloServer(typeDefs: DocumentNode, resolvers: any) {
  const app = express();
  const httpServer = http.createServer(app);
  const server = new ApolloServer({
    typeDefs,
    resolvers,
    csrfPrevention: true,
    cache: "bounded",
    context: ({req}) => ({
      auth: req.headers['authorization'] //example, not used
    }),
    plugins: [ApolloServerPluginDrainHttpServer({ httpServer })],
  });
  
  await server.start();
  server.applyMiddleware({ app });

  await new Promise<void>((resolve) =>
    httpServer.listen({ port: 4000 }, resolve)
  );
  console.log(`🚀 Server ready at http://localhost:4000${server.graphqlPath}`);
}

export default startApolloServer;
