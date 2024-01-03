import { app, HttpRequest, InvocationContext } from "@azure/functions"
import { GetTableInput } from "../utils/table"
import { RestaurantEntity } from "../entities/RestaurantEntity"

const tableInput = GetTableInput('restaurant')

app.http('listRestaurants', {
    trigger: {
        authLevel: 'anonymous',
        methods: ['GET'],
        route: 'restaurant',
        name: 'listRestaurants',
        type: 'httpTrigger',
    },
    handler(request: HttpRequest, context: InvocationContext)
    {
        const restaurants = (context.extraInputs.get(tableInput) as RestaurantEntity[])
            .map(x => JSON.parse(x.ClientData as any))
        return ({ jsonBody: restaurants })
    },
    extraInputs: [tableInput]
})
