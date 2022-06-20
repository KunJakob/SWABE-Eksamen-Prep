import { gql } from "apollo-server-core";

export default gql`
    type Query {
        hello: String
        users: [User]!
        user(id: ID!): User
    }
    type Mutation {
        createUser(input: CreateUserInput!): User
        updateUser(input: UpdateUserInput!): User
        deleteUser(id: ID!): User
    }
    type User {
        id: ID!
        name: String!
        email: String!
    }

    input CreateUserInput {
        name: String!
        email: String!
    }

    input UpdateUserInput {
        id: ID!
        name: String
        email: String
    }
`;
