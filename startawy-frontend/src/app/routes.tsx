import { createBrowserRouter, Navigate } from 'react-router-dom'
import { AppShell } from './shell/AppShell'
import { PublicLayout } from './shell/PublicLayout'
import { FounderLayout } from './shell/FounderLayout'
import { ConsultantLayout } from './shell/ConsultantLayout'
import { AdminLayout } from './shell/AdminLayout'
import { RequireAuth } from './shell/RequireAuth'
import { RoleGate } from './shell/RoleGate'

import { LandingPage } from '../pages/public/LandingPage'
import { LoginPage } from '../pages/public/LoginPage'
import { SignupPage } from '../pages/public/SignupPage'
import { RoleSwitcherPage } from '../pages/public/RoleSwitcherPage'
import { PlaceholderPage } from '../pages/_shared/PlaceholderPage'

import { NotFoundPage } from '../pages/errors/NotFoundPage'

export const router = createBrowserRouter([
  {
    element: <AppShell />,
    children: [
      {
        element: <PublicLayout />,
        children: [
          { path: '/', element: <LandingPage /> },
          { path: '/login', element: <LoginPage /> },
          { path: '/signup', element: <SignupPage /> },
          { path: '/role-switcher', element: <RoleSwitcherPage /> },
        ],
      },
      {
        element: (
          <RequireAuth>
            <FounderLayout />
          </RequireAuth>
        ),
        children: [
          {
            path: '/dashboard',
            element: (
              <RoleGate allow={['StartupFounder']}>
                <PlaceholderPage title="Founder Dashboard" />
              </RoleGate>
            ),
          },
          {
            path: '/plans',
            element: (
              <RoleGate allow={['StartupFounder']}>
                <PlaceholderPage title="Plans" />
              </RoleGate>
            ),
          },
          {
            path: '/my-plan',
            element: (
              <RoleGate allow={['StartupFounder']}>
                <PlaceholderPage title="My Plan" />
              </RoleGate>
            ),
          },
          {
            path: '/budget-analysis',
            element: (
              <RoleGate allow={['StartupFounder']}>
                <PlaceholderPage title="Budget Analysis" />
              </RoleGate>
            ),
          },
          {
            path: '/generate-budget-report',
            element: (
              <RoleGate allow={['StartupFounder']}>
                <PlaceholderPage title="Generate Budget Report" />
              </RoleGate>
            ),
          },
          {
            path: '/ai-chatbot',
            element: (
              <RoleGate allow={['StartupFounder']}>
                <PlaceholderPage title="AI Chatbot" />
              </RoleGate>
            ),
          },
          {
            path: '/market-reports',
            element: (
              <RoleGate allow={['StartupFounder']}>
                <PlaceholderPage title="Market Reports" />
              </RoleGate>
            ),
          },
          {
            path: '/startawy-library',
            element: (
              <RoleGate allow={['StartupFounder']}>
                <PlaceholderPage title="Startawy Library" />
              </RoleGate>
            ),
          },
          {
            path: '/book-consultant',
            element: (
              <RoleGate allow={['StartupFounder']}>
                <PlaceholderPage title="Book Consultant" />
              </RoleGate>
            ),
          },
          {
            path: '/book-session-professional',
            element: (
              <RoleGate allow={['StartupFounder']}>
                <PlaceholderPage title="Book Session (Professional)" />
              </RoleGate>
            ),
          },
          {
            path: '/consultant/:id',
            element: (
              <RoleGate allow={['StartupFounder']}>
                <PlaceholderPage title="Consultant Details" />
              </RoleGate>
            ),
          },
          {
            path: '/my-sessions',
            element: (
              <RoleGate allow={['StartupFounder']}>
                <PlaceholderPage title="My Sessions" />
              </RoleGate>
            ),
          },
          { path: '/profile', element: <PlaceholderPage title="Profile" /> },
          { path: '/edit-profile', element: <PlaceholderPage title="Edit Profile" /> },
          {
            path: '/feedback',
            element: (
              <RoleGate allow={['StartupFounder']}>
                <PlaceholderPage title="Feedback" />
              </RoleGate>
            ),
          },
          {
            path: '/my-payments',
            element: (
              <RoleGate allow={['StartupFounder']}>
                <PlaceholderPage title="My Payments" />
              </RoleGate>
            ),
          },
          {
            path: '/payment',
            element: (
              <RoleGate allow={['StartupFounder']}>
                <PlaceholderPage title="Payment" />
              </RoleGate>
            ),
          },
          {
            path: '/add-payment-method',
            element: (
              <RoleGate allow={['StartupFounder']}>
                <PlaceholderPage title="Add Payment Method" />
              </RoleGate>
            ),
          },
        ],
      },
      {
        element: (
          <RequireAuth>
            <ConsultantLayout />
          </RequireAuth>
        ),
        children: [
          {
            path: '/consultant/dashboard',
            element: (
              <RoleGate allow={['FinancialConsultant']}>
                <PlaceholderPage title="Consultant Dashboard" />
              </RoleGate>
            ),
          },
          {
            path: '/consultant/sessions',
            element: (
              <RoleGate allow={['FinancialConsultant']}>
                <PlaceholderPage title="Consultant Sessions" />
              </RoleGate>
            ),
          },
          {
            path: '/consultant/availability',
            element: (
              <RoleGate allow={['FinancialConsultant']}>
                <PlaceholderPage title="Consultant Availability" />
              </RoleGate>
            ),
          },
          {
            path: '/consultant/clients',
            element: (
              <RoleGate allow={['FinancialConsultant']}>
                <PlaceholderPage title="Consultant Clients" />
              </RoleGate>
            ),
          },
          {
            path: '/consultant/client/:id',
            element: (
              <RoleGate allow={['FinancialConsultant']}>
                <PlaceholderPage title="Client Details" />
              </RoleGate>
            ),
          },
          {
            path: '/consultant/client/:id/add-review',
            element: (
              <RoleGate allow={['FinancialConsultant']}>
                <PlaceholderPage title="Add Review" />
              </RoleGate>
            ),
          },
          {
            path: '/consultant/client/:id/schedule-meeting',
            element: (
              <RoleGate allow={['FinancialConsultant']}>
                <PlaceholderPage title="Schedule Meeting" />
              </RoleGate>
            ),
          },
          {
            path: '/consultant/client/:id/send-message',
            element: (
              <RoleGate allow={['FinancialConsultant']}>
                <PlaceholderPage title="Send Message" />
              </RoleGate>
            ),
          },
          {
            path: '/consultant/client/:id/generate-report',
            element: (
              <RoleGate allow={['FinancialConsultant']}>
                <PlaceholderPage title="Generate Report" />
              </RoleGate>
            ),
          },
          {
            path: '/consultant/earnings',
            element: (
              <RoleGate allow={['FinancialConsultant']}>
                <PlaceholderPage title="Earnings" />
              </RoleGate>
            ),
          },
          {
            path: '/consultant/recommendations',
            element: (
              <RoleGate allow={['FinancialConsultant']}>
                <PlaceholderPage title="Recommendations" />
              </RoleGate>
            ),
          },
          {
            path: '/consultant/startup-details',
            element: (
              <RoleGate allow={['FinancialConsultant']}>
                <PlaceholderPage title="Startup Details" />
              </RoleGate>
            ),
          },
          {
            path: '/consultant/follow-up-plans',
            element: (
              <RoleGate allow={['FinancialConsultant']}>
                <PlaceholderPage title="Follow-up Plans" />
              </RoleGate>
            ),
          },
          {
            path: '/consultant/profile',
            element: (
              <RoleGate allow={['FinancialConsultant']}>
                <PlaceholderPage title="Consultant Profile" />
              </RoleGate>
            ),
          },
          {
            path: '/consultant/edit-profile',
            element: (
              <RoleGate allow={['FinancialConsultant']}>
                <PlaceholderPage title="Edit Consultant Profile" />
              </RoleGate>
            ),
          },
        ],
      },
      {
        element: (
          <RequireAuth>
            <AdminLayout />
          </RequireAuth>
        ),
        children: [
          {
            path: '/admin/dashboard',
            element: (
              <RoleGate allow={['Administrator']}>
                <PlaceholderPage title="Admin Dashboard" />
              </RoleGate>
            ),
          },
          {
            path: '/admin/analytics',
            element: (
              <RoleGate allow={['Administrator']}>
                <PlaceholderPage title="Admin Analytics" />
              </RoleGate>
            ),
          },
          {
            path: '/admin/founders',
            element: (
              <RoleGate allow={['Administrator']}>
                <PlaceholderPage title="Manage Founders" />
              </RoleGate>
            ),
          },
          {
            path: '/admin/consultants',
            element: (
              <RoleGate allow={['Administrator']}>
                <PlaceholderPage title="Manage Consultants" />
              </RoleGate>
            ),
          },
          {
            path: '/admin/add-consultant',
            element: (
              <RoleGate allow={['Administrator']}>
                <PlaceholderPage title="Add Consultant" />
              </RoleGate>
            ),
          },
          {
            path: '/admin/packages',
            element: (
              <RoleGate allow={['Administrator']}>
                <PlaceholderPage title="Admin Packages" />
              </RoleGate>
            ),
          },
          {
            path: '/admin/feedback',
            element: (
              <RoleGate allow={['Administrator']}>
                <PlaceholderPage title="Review Feedback" />
              </RoleGate>
            ),
          },
          {
            path: '/admin/profile',
            element: (
              <RoleGate allow={['Administrator']}>
                <PlaceholderPage title="Admin Profile" />
              </RoleGate>
            ),
          },
          {
            path: '/admin/edit-profile',
            element: (
              <RoleGate allow={['Administrator']}>
                <PlaceholderPage title="Edit Admin Profile" />
              </RoleGate>
            ),
          },
        ],
      },

      // backend currently returns redirectTo like "/founder/dashboard"
      { path: '/founder/dashboard', element: <Navigate to="/dashboard" replace /> },
      { path: '*', element: <NotFoundPage /> },
    ],
  },
])

