import { useState } from 'react'
import './App.css'

type ApiResult = {
  service: string
  message?: string
  checkedAtUtc?: string
  apiResponse?: unknown
}

function App() {
  const [bffInfo, setBffInfo] = useState<ApiResult | null>(null)
  const [apiInfo, setApiInfo] = useState<ApiResult | null>(null)
  const [error, setError] = useState<string | null>(null)

  async function loadBffInfo() {
    setError(null)

    const response = await fetch('/bff/system/info')

    if (!response.ok) {
      setError(`BFF info failed with status ${response.status}`)
      return
    }

    setBffInfo(await response.json())
  }

  async function loadApiThroughBff() {
    setError(null)

    const response = await fetch('/bff/api-info')

    if (!response.ok) {
      setError(`API through BFF failed with status ${response.status}`)
      return
    }

    setApiInfo(await response.json())
  }

  return (
      <main style={{ maxWidth: 900, margin: '40px auto', fontFamily: 'Arial, sans-serif' }}>
        <h1>Deployment Demo</h1>

        <p>
          React SPA → BFF → API verification app.
        </p>

        <div style={{ display: 'flex', gap: 12, marginBottom: 24 }}>
          <button onClick={loadBffInfo}>Load BFF Info</button>
          <button onClick={loadApiThroughBff}>Load API Through BFF</button>
        </div>

        {error && (
            <pre style={{ background: '#ffe6e6', padding: 16 }}>
          {error}
        </pre>
        )}

        <section>
          <h2>BFF Info</h2>
          <pre style={{ background: '#f4f4f4', padding: 16 }}>
          {bffInfo ? JSON.stringify(bffInfo, null, 2) : 'Not loaded yet'}
        </pre>
        </section>

        <section>
          <h2>API Through BFF</h2>
          <pre style={{ background: '#f4f4f4', padding: 16 }}>
          {apiInfo ? JSON.stringify(apiInfo, null, 2) : 'Not loaded yet'}
        </pre>
        </section>
      </main>
  )
}

export default App