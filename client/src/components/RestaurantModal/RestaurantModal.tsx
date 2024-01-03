import { FormEvent } from 'react'
import { DeliveryServiceType } from 'what-to-eat'

export default function RestaurantModal() {
  const onSubmit = (evt: FormEvent) => {
    console.log(new FormData(evt.currentTarget as HTMLFormElement))
  }
  return (
    <form onSubmit={onSubmit}>
      <input name="name" />
      <br />
      <input name="url" type="url" />
      <br />
      {Object.entries(DeliveryServiceType).map(([key, value]) => {
        return (
          <>
            <label>
              {key}: <input name={value} type="url" />
            </label>
            <br />
          </>
        )
      })}
      <button>Create Restaurant</button>
    </form>
  )
}
