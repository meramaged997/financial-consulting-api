import { DashLayout } from './_DashLayout'

export function ConsultantLayout() {
  return (
    <DashLayout
      title="Consultant Dashboard"
      links={[
        { to: '/consultant/dashboard', label: 'Dashboard' },
        { to: '/consultant/sessions', label: 'Sessions' },
        { to: '/consultant/availability', label: 'Availability' },
        { to: '/consultant/clients', label: 'Clients' },
        { to: '/consultant/earnings', label: 'Earnings' },
        { to: '/consultant/recommendations', label: 'Recommendations' },
        { to: '/consultant/follow-up-plans', label: 'Follow-up Plans' },
        { to: '/consultant/profile', label: 'Profile' },
      ]}
    />
  )
}

