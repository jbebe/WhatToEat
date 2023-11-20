import styles from './RestaurantList.module.scss'

export default function RestaurantList() {
    return (
      <table className={styles.table}>
        <thead>
            <tr>
                <th>Name</th>
                <th>Action</th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td>Green Thai</td>
                <td>
                    <button>Go</button>
                    <button>Order</button>
                </td>
            </tr>
            <tr>
                <td>TyúkÁsz</td>
                <td>
                    <button>Go</button>
                    <button disabled>Order</button>
                </td>
            </tr>
        </tbody>
      </table>
    )
  }
  