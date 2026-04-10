import { DashLayout } from './_DashLayout'

export function AdminLayout() {
  return (
    <DashLayout
      title="Admin Dashboard"
      links={[
        { to: '/admin/dashboard', label: 'Dashboard' },
        { to: '/admin/analytics', label: 'Analytics' },
        { to: '/admin/founders', label: 'Founders' },
        { to: '/admin/consultants', label: 'Consultants' },
        { to: '/admin/add-consultant', label: 'Add Consultant' },
        { to: '/admin/packages', label: 'Packages' },
        { to: '/admin/feedback', label: 'Feedback' },
        { to: '/admin/profile', label: 'Profile' },
      ]}
    />
  )
}

