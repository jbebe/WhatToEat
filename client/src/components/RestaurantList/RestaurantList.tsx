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
import { Restaurant } from 'what-to-eat'
import { useQuery } from 'react-query'

export default function RestaurantList() {
  let { data, isSuccess } = useQuery('restaurant', async () => {
    return (await (await fetch('/api/restaurant')).json()) as Restaurant[]
  })
  const [sorting, setSorting] = React.useState<SortingState>([])
  const columns = React.useMemo<ColumnDef<Restaurant>[]>(
    () => [
      {
        header: 'Name',
        accessorKey: 'name',
        cell: (info) => {
          const row = info.row.original
          return (
            <>
              {row.url ? <a href={row.url}>{row.name}</a> : row.name}
              {Object.entries(row.delivery ?? {}).map(([_, url]) => {
                return (
                  <>
                    <a href={url}>
                      <img src="#" />
                    </a>
                  </>
                )
              })}
            </>
          )
        },
      },
      {
        header: 'Distance',
        accessorKey: 'distance',
        cell: (info) => {
          const dist = info.getValue() as number | undefined
          if (!dist) return '-'
          return dist >= 1
            ? `${Math.round(dist)} km`
            : `${Math.round(dist * 1000)} m `
        },
      },
      {
        header: 'Action',
        cell: (_) => (
          <>
            <button>Go</button> <button>Order</button>
          </>
        ),
      },
    ],
    []
  )

  const table = useReactTable({
    data: data ?? [],
    columns,
    state: {
      sorting,
    },
    onSortingChange: setSorting,
    getCoreRowModel: getCoreRowModel(),
    getSortedRowModel: getSortedRowModel(),
    debugTable: true,
  })

  if (!isSuccess) return <div>Failed to load restaurants</div>

  return (
    <table className={styles.table}>
      <thead>
        {table.getHeaderGroups().map((headerGroup) => (
          <tr key={headerGroup.id}>
            {headerGroup.headers.map((header) => {
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
        {table.getRowModel().rows.map((row) => {
          return (
            <tr key={row.id}>
              {row.getVisibleCells().map((cell) => {
                return (
                  <td key={cell.id}>
                    {flexRender(cell.column.columnDef.cell, cell.getContext())}
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
