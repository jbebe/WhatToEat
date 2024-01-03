import { User, UserRequest } from "what-to-eat"
import { BaseEntity } from "../utils/table"
import { createId } from "../utils/id"

export interface UserEntity extends BaseEntity<User> {
    PartitionKey: 'user',
}

export function createUserEntity(request: UserRequest): UserEntity {
    const id = createId()
    return {
        PartitionKey: 'user',
        RowKey: id,
        ClientData: {
            id,
            ...request,
        },
    }
}
