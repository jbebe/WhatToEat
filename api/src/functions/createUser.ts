import { app, HttpRequest, HttpResponseInit, InvocationContext } from "@azure/functions";
import { TableOutput } from "../utils/table"
import { UserRequestSchema } from "what-to-eat";
import { createUserEntity } from "../entities/UserEntity";

app.http('createUser', {
    trigger: {
        authLevel: 'anonymous',
        methods: ['POST'],
        route: 'user',
        name: 'createUser',
        type: 'httpTrigger',
    },
    async handler(request: HttpRequest, context: InvocationContext): Promise<HttpResponseInit> {
        const req = UserRequestSchema.parse(await request.json())
        const entity = createUserEntity(req)
        context.extraOutputs.set(TableOutput, entity)
        return { jsonBody: entity.ClientData }
    },
    extraOutputs: [TableOutput]
});
