<script setup lang="ts">
import QuickActionMockup from './QuickActionMockup.vue'
</script>

<template>
  <QuickActionMockup
    primitive="int"
    class-name="Age"
    :menu="[
      { label: 'Generate constructor from members...' },
      { label: `Generate constructor 'Age()'` },
      { label: 'Extract interface...' },
      { label: 'Extract base class...' },
      { label: 'Implement Define()' },
      { label: 'Implement Define() explicitly', selected: true },
      { label: 'Generate Equals(T) and GetHashCode()' },
      { label: 'Generate ToString()' },
    ]"
  >
    <div class="line indent-4 added"><span class="plus">+</span><span class="keyword">static</span> <span class="type">StrongTypeDefinition</span>&lt;<span class="keyword">int</span>&gt; <span class="type">IStrongType</span>&lt;<span class="type">Age</span>, <span class="keyword">int</span>&gt;.<span class="method">Define</span>() => <span class="type">StrongType</span>.<span class="method">Define</span>&lt;<span class="keyword">int</span>&gt;();</div>
  </QuickActionMockup>
</template>
