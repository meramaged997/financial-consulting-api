import { useMemo, useState } from 'react'
import type { FormEvent } from 'react'
import { Link, useNavigate } from 'react-router-dom'
import { useAuth } from '../../app/state/auth'

export function SignupPage() {
  const auth = useAuth()
  const nav = useNavigate()

  const [fullName, setFullName] = useState('Test User')
  const [email, setEmail] = useState('test2@example.com')
  const [password, setPassword] = useState('Test@1234')
  const [confirmPassword, setConfirmPassword] = useState('Test@1234')
  const [role, setRole] = useState<1 | 2 | 3>(1)
  const [msg, setMsg] = useState('')
  const [busy, setBusy] = useState(false)

  const canSubmit = useMemo(() => {
    return (
      fullName.trim().length > 2 &&
      email.trim().length > 3 &&
      password.length >= 6 &&
      password === confirmPassword &&
      !busy
    )
  }, [busy, confirmPassword, email, fullName, password])

  async function onSubmit(e: FormEvent) {
    e.preventDefault()
    setBusy(true)
    setMsg('')
    try {
      const res = await auth.register({
        fullName,
        email,
        password,
        confirmPassword,
        phoneNumber: '01000000000',
        role,
      })
      if (!res.success) {
        setMsg(res.message)
        return
      }
      nav(res.data.redirectTo || '/', { replace: true })
    } finally {
      setBusy(false)
    }
  }

  return (
    <div style={styles.wrap}>
      <div style={styles.card}>
        <h1 style={styles.h1}>Create account</h1>
        <p style={styles.p}>اختاري الدور وهتتحولي للـDashboard المناسب بعد التسجيل.</p>

        <form onSubmit={onSubmit} style={{ display: 'grid', gap: 10 }}>
          <label style={styles.label}>
            Full name
            <input value={fullName} onChange={(e) => setFullName(e.target.value)} style={styles.input} />
          </label>
          <label style={styles.label}>
            Email
            <input value={email} onChange={(e) => setEmail(e.target.value)} style={styles.input} />
          </label>
          <label style={styles.label}>
            Password
            <input value={password} onChange={(e) => setPassword(e.target.value)} style={styles.input} type="password" />
          </label>
          <label style={styles.label}>
            Confirm password
            <input
              value={confirmPassword}
              onChange={(e) => setConfirmPassword(e.target.value)}
              style={styles.input}
              type="password"
            />
          </label>
          <label style={styles.label}>
            Role
            <select value={role} onChange={(e) => setRole(Number(e.target.value) as 1 | 2 | 3)} style={styles.input}>
              <option value={1}>Startup Founder</option>
              <option value={2}>Financial Consultant</option>
              <option value={3}>Administrator</option>
            </select>
          </label>

          <button disabled={!canSubmit} style={styles.primary}>
            {busy ? 'Creating…' : 'Sign up'}
          </button>
        </form>

        <div style={{ display: 'flex', justifyContent: 'space-between', marginTop: 10 }}>
          <Link to="/login" style={styles.link}>
            I already have an account
          </Link>
          <Link to="/" style={styles.link}>
            Back to home
          </Link>
        </div>

        {msg ? <div style={styles.msg}>{msg}</div> : null}
      </div>
    </div>
  )
}

const styles: Record<string, React.CSSProperties> = {
  wrap: { display: 'grid', placeItems: 'center', minHeight: '70vh' },
  card: {
    width: 'min(560px, 100%)',
    border: '1px solid rgba(2,6,23,0.08)',
    background: '#fff',
    borderRadius: 18,
    padding: 16,
  },
  h1: { margin: 0, color: '#0f172a' },
  p: { margin: '6px 0 12px', color: '#475569' },
  label: { display: 'grid', gap: 6, fontWeight: 700, color: '#0f172a' },
  input: {
    padding: '10px 12px',
    borderRadius: 12,
    border: '1px solid rgba(2,6,23,0.12)',
    outline: 'none',
  },
  primary: {
    padding: '10px 12px',
    borderRadius: 12,
    border: 'none',
    background: '#0f766e',
    color: '#fff',
    fontWeight: 900,
    cursor: 'pointer',
  },
  link: { color: '#0f766e', fontWeight: 800, textDecoration: 'none' },
  msg: {
    marginTop: 12,
    padding: 10,
    borderRadius: 12,
    background: '#fff7ed',
    border: '1px solid rgba(234,88,12,0.25)',
    color: '#9a3412',
    fontWeight: 750,
  },
}

