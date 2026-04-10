export type ApiEnvelope<T> = {
  success: boolean
  message: string
  data: T
  errors: string[] | null
}

export async function apiFetch<T>(
  path: string,
  options: RequestInit & { token?: string } = {},
): Promise<ApiEnvelope<T>> {
  const { token, headers, ...rest } = options
  const res = await fetch(path.startsWith('/') ? path : `/${path}`, {
    ...rest,
    headers: {
      'Content-Type': 'application/json',
      ...(token ? { Authorization: `Bearer ${token}` } : null),
      ...(headers ?? {}),
    },
  })

  const json = (await res.json()) as ApiEnvelope<T>
  return json
}

