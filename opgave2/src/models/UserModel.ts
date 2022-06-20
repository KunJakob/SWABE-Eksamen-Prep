import { Entity, OneToMany, PrimaryKey, Property } from "@mikro-orm/core";
import {v4} from "uuid";
@Entity()
export class UserModel {
  @PrimaryKey()
  id: string = v4();

  @Property()
  name!: string;

  @Property()
  email!: string;

  constructor(name: string, email: string) {
    this.name = name;
    this.email = email;
  }
}
