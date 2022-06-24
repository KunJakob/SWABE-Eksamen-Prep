import { UserModel } from "./../../../opgave1/src/models/UserModel";
import { createUser } from "../services/UserService";
import {} from "graphql-tools";
import {
  getAllUsers,
  updateUser,
  findOneUser,
} from "../services/UserService";

export default {
  Query: {
    users: async (parent: void, args: void, ctx: any, info: any) => {
      return await getAllUsers();
    },
    user: async (parent: void, args: { id: string }) => {
      return await findOneUser(args.id);
    },
  },
  Mutation: {
    createUser: async (
      parent: void,
      args: { input: { name: string; email: string } }
    ) => {
      return await createUser(args.input.name, args.input.email);
    },
    updateUser: async (
      parent: void,
      args: { input: { id: string; name?: string; email?: string } }
    ) => {
      return await updateUser(args.input.id, args.input.name, args.input.email);
    },
  },
  User: {
    id: (user: UserModel) => user.id,
    //name: (user: UserModel) => user.name,
    email: (user: UserModel) => user.email,
  },
};
