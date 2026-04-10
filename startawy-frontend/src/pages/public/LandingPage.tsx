import { useState } from 'react'
import { Link } from 'react-router-dom'
import reactLogo from '../../assets/react.svg'
import viteLogo from '../../assets/vite.svg'
import heroImg from '../../assets/hero.png'
import '../../App.css'

export function LandingPage() {
  const [count, setCount] = useState(0)

  return (
    <>
      <section id="center">
        <div className="hero">
          <img src={heroImg} className="base" width="170" height="179" alt="" />
          <img src={reactLogo} className="framework" alt="React logo" />
          <img src={viteLogo} className="vite" alt="Vite logo" />
        </div>
        <div style={{ textAlign: 'center' }}>
          <h1 style={{ marginTop: 0 }}>Startawy</h1>
          <p style={{ margin: '6px 0 14px', color: 'var(--text)' }}>
            Web app جاهز: Routing + Auth + Dashboards
          </p>
          <div style={{ display: 'flex', gap: 10, justifyContent: 'center', flexWrap: 'wrap' }}>
            <Link to="/signup" style={btnPrimary}>
              Sign up
            </Link>
            <Link to="/login" style={btnSecondary}>
              Login
            </Link>
            <Link to="/plans" style={btnSecondary}>
              Plans
            </Link>
          </div>
        </div>
        <button className="counter" onClick={() => setCount((c) => c + 1)}>
          Count is {count}
        </button>
      </section>

      <div className="ticks"></div>

      <section id="next-steps">
        <div id="docs">
          <svg className="icon" role="presentation" aria-hidden="true">
            <use href="/icons.svg#documentation-icon"></use>
          </svg>
          <h2>Dashboards</h2>
          <p>روابط سريعة بعد تسجيل الدخول</p>
          <ul>
            <li>
              <Link to="/dashboard">
                <img className="logo" src={viteLogo} alt="" />
                Founder
              </Link>
            </li>
            <li>
              <Link to="/consultant/dashboard">
                <img className="logo" src={reactLogo} alt="" />
                Consultant
              </Link>
            </li>
          </ul>
        </div>
        <div id="social">
          <svg className="icon" role="presentation" aria-hidden="true">
            <use href="/icons.svg#social-icon"></use>
          </svg>
          <h2>Admin</h2>
          <p>إدارة المستخدمين والباقات والملاحظات</p>
          <ul>
            <li>
              <Link to="/admin/dashboard">
                <svg className="button-icon" role="presentation" aria-hidden="true">
                  <use href="/icons.svg#github-icon"></use>
                </svg>
                Admin dashboard
              </Link>
            </li>
            <li>
              <Link to="/role-switcher">
                <svg className="button-icon" role="presentation" aria-hidden="true">
                  <use href="/icons.svg#discord-icon"></use>
                </svg>
                Role switcher (demo)
              </Link>
            </li>
          </ul>
        </div>
      </section>

      <div className="ticks"></div>
      <section id="spacer"></section>
    </>
  )
}

const btnPrimary: React.CSSProperties = {
  background: '#0f766e',
  color: '#fff',
  fontWeight: 900,
  padding: '10px 14px',
  borderRadius: 12,
  textDecoration: 'none',
}

const btnSecondary: React.CSSProperties = {
  background: '#fff',
  color: '#0f172a',
  fontWeight: 900,
  padding: '10px 14px',
  borderRadius: 12,
  border: '1px solid rgba(2,6,23,0.12)',
  textDecoration: 'none',
}

