import { DashLayout } from './_DashLayout'

export function FounderLayout() {
  return (
    <DashLayout
      title="Founder Dashboard"
      links={[
        { to: '/dashboard', label: 'Dashboard' },
        { to: '/plans', label: 'Plans' },
        { to: '/my-plan', label: 'My Plan' },
        { to: '/budget-analysis', label: 'Budget Analysis' },
        { to: '/ai-chatbot', label: 'AI Chatbot' },
        { to: '/market-reports', label: 'Market Reports' },
        { to: '/book-consultant', label: 'Book Consultant' },
        { to: '/my-sessions', label: 'My Sessions' },
        { to: '/profile', label: 'Profile' },
        { to: '/my-payments', label: 'Payments' },
        { to: '/feedback', label: 'Feedback' },
      ]}
    />
  )
}

