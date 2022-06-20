import { UserModel } from './../../../opgave1/src/models/UserModel';
import { createUser } from "../services/UserService";
import { Router } from "express";
import {} from "graphql-tools"
import {
  deleteUser,
  getAllUsers,
  updateUser,
  findOneUser,
} from "../services/UserService";


export default { 
  Query: {
    users: async () => {
      return await getAllUsers();
    },
    user: async (_: void, args: {id: string}) => {
      return await findOneUser(args.id);
    }
  },
  Mutation: {
    createUser: async (_: void, args: {input: {name: string, email: string}}) => {
      return await createUser(args.input.name, args.input.email);
    },
    updateUser: async (_: void, args: {input: {id: string, name?: string, email?: string}}) => {
      return await updateUser(args.input.id, args.input.name, args.input.email);
    }
  },
  User: {
    id: (user: UserModel) => user.id,
    //name: (user: UserModel) => user.name,
    email: (user: UserModel) => user.email,
  }
}




















interface UpdateUserDto {
  id: string;
  name?: string;
  email?: string;
}

const routes = Router();

routes.get("/", async (req, res) => {
  const users = await getAllUsers();
  if (!users) {
    res.status(500).json({ message: "Something went wrong" });
  } else {
    res.json(users);
  }
});

routes.post("/", async (req, res) => {
  if (!req.body.name || !req.body.email) {
    res.status(400);
    return res.json({ message: "One of `name, email` is missing" });
  }
  const user = await createUser(req.body.name, req.body.email);
  res.json(user);
});

routes.get("/:id", async (req, res) => {
  const users = await findOneUser(req.params.id);
  if (!users) {
    res.status(404).json({ message: "No user with that ID was found" });
  } else {
    res.json(users);
  }
});

routes.patch("/", async (req, res) => {
  const { id, name, email } = req.body;
  if (!id) {
    return res.status(400).json({ message: "`id` is missing" });
  } else if (!name && !email) {
    return res
      .status(400)
      .json({ message: " one of `name, email` is missing" });
  }
  const body: UpdateUserDto = req.body;
  try {
    const user = await updateUser(body.id, body.name, body.email);
    res.json(user);
  } catch (error) {
    res.status(404).json({ message: "No user with that ID was found" });
  }
});

routes.delete("/:id", async (req, res) => {
  try {
    const user = await deleteUser(req.params.id);
    res.json(user);
  } catch (error) {
    res.status(500).json({ message: "Something went wrong" });
  }
});

