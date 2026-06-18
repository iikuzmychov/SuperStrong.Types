import { readFileSync } from 'node:fs'
import { fileURLToPath } from 'node:url'
import { defineConfig } from 'vitepress'

const nugetIcon = readFileSync(
  fileURLToPath(new URL('./icons/nuget.svg', import.meta.url)),
  'utf-8',
)

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
  title: 'SuperStrong.Types',
  description: 'Stong types for .NET — define once, use everywhere!',
  lang: 'en-US',
  base: '/SuperStrong/',
  srcDir: 'src',
  cleanUrls: true,
  lastUpdated: true,
  head: [
    ['link', { rel: 'icon', type: 'image/png', href: '/SuperStrong/img/logo.png' }],
  ],
  vite: {
    publicDir: '../public',
  },
  sitemap: {
    hostname: 'https://iikuzmychov.github.io/SuperStrong/',
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
        ],
      },
      {
        text: 'Integrations',
        items: [
          { text: 'EF Core', link: '/integrations/ef-core' },
          { text: 'HotChocolate', link: '/integrations/hotchocolate' },
        ],
      },
      {
        text: 'Alternatives',
        items: [
          { text: 'vs Vogen', link: '/alternatives/vogen' },
          { text: 'vs StronglyTypedId', link: '/alternatives/strongly-typed-id' },
        ],
      },
    ],

    socialLinks: [
      { icon: 'github', link: 'https://github.com/iiKuzmychov/SuperStrong' },
      {
        icon: { svg: nugetIcon },
        link: 'https://www.nuget.org/packages/SuperStrong.Types',
        ariaLabel: 'NuGet',
      },
    ],

    search: {
      provider: 'local',
    },

    editLink: {
      pattern: 'https://github.com/iiKuzmychov/SuperStrong/edit/master/docs/:path',
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
})
