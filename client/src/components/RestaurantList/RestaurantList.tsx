import React from 'react'
import styles from './RestaurantList.module.scss'
import {
  ColumnDef,
  flexRender,
  getCoreRowModel,
  getSortedRowModel,
  SortingState,
  useReactTable,
} from '@tanstack/react-table'
import { Restaurant } from '../../types'

export default function RestaurantList() {
    const [sorting, setSorting] = React.useState<SortingState>([])
    const columns = React.useMemo<ColumnDef<Restaurant>[]>(
      () => [
        { header: 'Name', accessorKey: 'name', cell: info => {
          const row = info.row.original;
          return <>
            {row.url ? <a href={row.url}>{row.name}</a> : row.name}
            {Object.entries(row.delivery ?? {}).map(([type, url]) => {
              return <>
                <a href={url}><img src='#' /></a>
              </>
            })}
          </>
        } },
        { header: 'Distance', accessorKey: 'distance', cell: info => {
          const dist = info.getValue() as (number | undefined);
          if (!dist) return '-'
          return dist >= 1 ? `${Math.round(dist)} km` : `${Math.round(dist*1000)} m `
        } },
        { header: 'Action', cell: _ => <><button>Go</button> <button>Order</button></> },
      ],
      []
    )
    const [data] = React.useState(() => {
      const values = [...new Array(20)]
      .map(_ => ({ 
        name: btoa((Math.random()*1e14).toString()).toLowerCase().substring(0, 8),
        distance: Math.random() < .9 ? (Math.random()*20|0)/10 + .1 : 0,
        url: Math.random() < .8 ? '#' : undefined,
        delivery: Math.random() < .8 ? {
          'wolt': '#'
        } : undefined
      } as Restaurant))
      .sort((a, b) => a.name.localeCompare(b.name))
      return values
    })
    const table = useReactTable({
      data,
      columns,
      state: {
        sorting,
      },
      onSortingChange: setSorting,
      getCoreRowModel: getCoreRowModel(),
      getSortedRowModel: getSortedRowModel(),
      debugTable: true,
    })

  return (
    <table className={styles.table}>
        <thead>
          {table.getHeaderGroups().map(headerGroup => (
            <tr key={headerGroup.id}>
              {headerGroup.headers.map(header => {
                return (
                  <th key={header.id} colSpan={header.colSpan}>
                    {header.isPlaceholder ? null : (
                      <div
                        {...{
                          className: header.column.getCanSort()
                            ? 'cursor-pointer select-none'
                            : '',
                          onClick: header.column.getToggleSortingHandler(),
                        }}
                      >
                        {flexRender(
                          header.column.columnDef.header,
                          header.getContext()
                        )}
                        {{
                          asc: ' 🔼',
                          desc: ' 🔽',
                        }[header.column.getIsSorted() as string] ?? null}
                      </div>
                    )}
                  </th>
                )
              })}
            </tr>
          ))}
        </thead>
        <tbody>
          {table
            .getRowModel()
            .rows
            .map(row => {
              return (
                <tr key={row.id}>
                  {row.getVisibleCells().map(cell => {
                    return (
                      <td key={cell.id}>
                        {flexRender(
                          cell.column.columnDef.cell,
                          cell.getContext()
                        )}
                      </td>
                    )
                  })}
                </tr>
              )
            })}
        </tbody>
      </table>
  )
}