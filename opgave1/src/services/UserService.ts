import { DI } from "../index";

export const getAllUsers = async () => {
  return await DI.userRepository.findAll();
};

export const findOneUser = async (id: string) => {
  return await DI.userRepository.findOne(id);
};

export const createUser = async (name: string, email: string) => {
  const user = DI.userRepository.create({ name, email });
  DI.userRepository.persistAndFlush(user);
  return user;
};

export const deleteUser = async (id: string) => {
  const user = await DI.userRepository.findOne(id);
  if (user) {
    await DI.userRepository.removeAndFlush(user);
    return user;
  }
  throw new Error("No user by that ID");
};

export const updateUser = async (id: string, name?: string, email?: string) => {
  const user = await DI.userRepository.findOne(id);

  if (user) {
    user.name = name || user.name;
    user.email = email || user.email;
    await DI.userRepository.flush();
    return user;
  }
  throw new Error("No user by that ID");
};
