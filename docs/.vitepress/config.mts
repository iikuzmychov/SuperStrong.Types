import { defineConfig, type HeadConfig } from 'vitepress'

const hostname = 'https://superstrong.dev'
const siteName = 'SuperStrong.Types'
const siteDescription = 'Strong types for .NET — define once, use everywhere!'
const ogImage = `${hostname}/img/og-image.png`

function pageUrl(relativePath: string): string {
  const path = relativePath.replace(/index\.md$/, '').replace(/\.md$/, '')

  return `${hostname}/${path}`
}

async function resolveVersion(): Promise<string> {
  if (process.env.SUPERSTRONG_TYPES_VERSION) {
    return process.env.SUPERSTRONG_TYPES_VERSION
  }

  const response = await fetch(
    'https://api.nuget.org/v3-flatcontainer/superstrong.types/index.json',
  )

  if (!response.ok) {
    throw new Error(`Could not fetch versions from NuGet (HTTP ${response.status}).`)
  }

  const data = (await response.json()) as { versions?: string[] }
  const version = data.versions?.at(-1)

  if (!version) {
    throw new Error('NuGet returned no versions for SuperStrong.Types.')
  }

  return version
}

const version = await resolveVersion()

export default defineConfig({
  title: siteName,
  description: siteDescription,
  lang: 'en-US',
  srcDir: 'src',
  cleanUrls: true,
  lastUpdated: true,
  head: [
    ['link', { rel: 'icon', type: 'image/png', href: '/img/logo.png' }],
    ['meta', { property: 'og:type', content: 'website' }],
    ['meta', { property: 'og:site_name', content: siteName }],
    ['meta', { property: 'og:image', content: ogImage }],
    ['meta', { property: 'og:image:width', content: '1200' }],
    ['meta', { property: 'og:image:height', content: '630' }],
    ['meta', { name: 'twitter:card', content: 'summary_large_image' }],
    ['meta', { name: 'twitter:image', content: ogImage }],
  ],
  vite: {
    publicDir: '../public',
  },
  sitemap: {
    hostname: `${hostname}/`,
  },
  themeConfig: {
    logo: '/img/logo.png',

    sidebar: [
      {
        text: 'Preface',
        items: [
          { text: 'Primitive Obsession', link: '/preface/primitive-obsession' },
        ],
      },
      {
        text: 'Tutorial',
        items: [
          { text: 'Getting Started', link: '/tutorial/getting-started' },
          { text: 'Validation', link: '/tutorial/validation' },
          { text: 'Customization', link: '/tutorial/customization' },
          { text: 'Null Handling', link: '/tutorial/null-handling' },
        ],
      },
      {
        text: 'Integrations',
        items: [
          { text: 'OpenAPI', link: '/integrations/openapi' },
          { text: 'EF Core', link: '/integrations/ef-core' },
          { text: 'Hot Chocolate', link: '/integrations/hotchocolate' },
          { text: 'System.Text.Json', link: '/integrations/system-text-json' },
          { text: 'Newtonsoft.Json', link: '/integrations/newtonsoft-json' },
        ],
      },
    ],

    socialLinks: [
      {
        icon: 'github',
        link: 'https://github.com/iikuzmychov/SuperStrong.Types' },
      {
        icon: 'nuget',
        link: 'https://www.nuget.org/packages/SuperStrong.Types',
      },
    ],

    search: {
      provider: 'local',
    },

    lastUpdated: {
      formatOptions: {
        dateStyle: 'medium',
        timeStyle: 'short',
      },
    },

    editLink: {
      pattern: 'https://github.com/iikuzmychov/SuperStrong.Types/edit/master/docs/src/:path',
      text: 'Edit this page on GitHub',
    },

    footer: {
      message: 'Released under the MIT License.',
      copyright: 'Copyright © Ihor Kuzmychov',
    },
  },

  transformPageData(pageData) {
    pageData.frontmatter.version = version
  },

  transformHead({ pageData }) {
    const title = pageData.frontmatter.title || pageData.title || siteName
    const description = pageData.frontmatter.description || pageData.description || siteDescription
    const url = pageUrl(pageData.relativePath)

    const head: HeadConfig[] = [
      ['link', { rel: 'canonical', href: url }],
      ['meta', { property: 'og:title', content: title }],
      ['meta', { property: 'og:description', content: description }],
      ['meta', { property: 'og:url', content: url }],
      ['meta', { name: 'twitter:title', content: title }],
      ['meta', { name: 'twitter:description', content: description }],
    ]

    return head
  },
})
