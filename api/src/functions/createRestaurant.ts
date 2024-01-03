import { app, HttpRequest, HttpResponseInit, InvocationContext } from "@azure/functions"
import { RestaurantRequestSchema } from "what-to-eat"
import { createRestaurantEntity } from "../entities/RestaurantEntity"
import { TableOutput } from "../utils/table"

app.http('createRestaurant', {
    trigger: {
        authLevel: 'anonymous',
        methods: ['POST'],
        route: 'restaurant',
        name: 'createRestaurant',
        type: 'httpTrigger',
    },
    async handler(request: HttpRequest, context: InvocationContext): Promise<HttpResponseInit> {
        const req = RestaurantRequestSchema.parse(await request.json())
        const entity = createRestaurantEntity(req)
        context.extraOutputs.set(TableOutput, entity)
        return { jsonBody: entity.ClientData }
    },
    extraOutputs: [TableOutput]
})
