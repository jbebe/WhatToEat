import React, { FormEvent } from 'react'
import { DeliveryServiceType } from '@lib/DeliveryServiceType'
import Card from '@shared/Card/Card'
import { Restaurant, RestaurantRequest, RestaurantRequestSchema } from '@lib/Restaurant'
import { useMutation, useQueryClient } from 'react-query'

export default function RestaurantModal() {
  const queryClient = useQueryClient()
  const mutation = useMutation(async (data: RestaurantRequest) => {
    const result = await fetch('/api/restaurant', {
      method: 'POST',
      body: JSON.stringify(data)
    })
    return (await result.json() as Restaurant)
  }, {
    onSuccess: () => {
      queryClient.invalidateQueries('restaurant')
    },
  })

  const onSubmit = (evt: FormEvent) => {
    evt.preventDefault()
    const data = new FormData(evt.currentTarget as HTMLFormElement)
    const request: RestaurantRequest = {
      name: data.get('name') as string,
      url: data.get('url') as string,
      delivery: Object.fromEntries(
        Object.values(DeliveryServiceType)
          .map(x => [x, data.get('delivery.' + x)])
          .filter(([, val]) => !!val)
      )
    }
    const validation = RestaurantRequestSchema.safeParse(request)
    if (!validation.success){
      alert(validation.error)
      return
    }
    
    mutation.mutate(request)
  }
  return (
    <Card title='Create restaurant'>
      <form onSubmit={onSubmit}>
        <label>Name*: <input name="name" /></label>
        <br />
        <label>Url: <input name="url" type="url" /></label>
        <br />
        {Object.entries(DeliveryServiceType)
          .filter(([, value]) => value !== DeliveryServiceType.OnFoot)
          .map(([key, value]) => {
            return (
              <React.Fragment key={value}>
                <label>
                  {key} url: <input name={`delivery.${value}`} type="url" />
                </label>
                <br />
              </React.Fragment>
            )
          })}
        <button>Create Restaurant</button>
      </form>
    </Card>
  )
}
