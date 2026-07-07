<script setup lang="ts">
const props = defineProps<{
  primitive: string
  className: string
  menu: { label: string; selected?: boolean; chevron?: boolean }[]
}>()

const selectedLabel = props.menu.find(item => item.selected)?.label
</script>

<template>
  <div class="vs-qa">
    <div class="ide" role="img" :aria-label="`${selectedLabel} quick action in Visual Studio`">
      <div class="editor">
        <div class="line">[<span class="type">StrongType</span>&lt;<span class="keyword">{{ primitive }}</span>&gt;]</div>
        <div class="codelens">0 references</div>
        <div class="line"><span class="keyword">public sealed partial class</span> <span class="classname">{{ className }}</span></div>
        <div class="line">{</div>
      </div>
      <div class="popup">
        <div class="menu">
          <div v-for="item in menu" :key="item.label" class="menu-item" :class="{ selected: item.selected }">
            {{ item.label }}<span v-if="item.selected || item.chevron" class="chevron">&#8250;</span>
          </div>
        </div>
        <div class="preview">
          <div class="code">
            <div class="line indent-2">...</div>
            <div class="line indent-2">{</div>
            <slot />
            <div class="line indent-2">}</div>
          </div>
          <div class="link">Preview changes</div>
        </div>
      </div>
    </div>
  </div>
</template>

<style>
.vs-qa {
  overflow-x: auto;
}

.vs-qa .ide {
  --editor-bg: #1E1E1E;
  --popup-bg: #1B1B1C;
  --popup-border: #3F3F46;
  --separator: #2D2D30;
  --selection-bg: #333334;
  --ui-text: #F1F1F1;
  --code-text: #DCDCDC;
  --keyword: #569CD6;
  --type: #4EC9B0;
  --method: #DCDCA3;
  --codelens: #999999;
  --link: #4E9FDA;
  --added-bg: rgba(70, 149, 74, 0.28);
  --added-plus: #57A64A;

  width: max-content;
  pointer-events: none;
  user-select: none;
  -webkit-user-select: none;
  padding: 6px 12px 16px 10px;
  border-radius: 6px;
  background: var(--editor-bg);
  color: var(--code-text);
  font-family: Consolas, 'Cascadia Mono', ui-monospace, Menlo, monospace;
  font-size: 14px;
  line-height: 19px;
}

.vs-qa .line {
  position: relative;
  white-space: nowrap;
}

.vs-qa .indent-2 {
  padding-left: 2ch;
}

.vs-qa .indent-4 {
  padding-left: 4ch;
}

.vs-qa .codelens {
  font-family: 'Segoe UI', -apple-system, sans-serif;
  font-size: 10px;
  line-height: 14px;
  color: var(--codelens);
}

.vs-qa .keyword {
  color: var(--keyword);
}

.vs-qa .type {
  color: var(--type);
}

.vs-qa .method {
  color: var(--method);
}

.vs-qa .classname {
  padding: 0 1px;
  background: var(--type);
  color: var(--editor-bg);
}

.vs-qa .popup {
  display: flex;
  align-items: flex-start;
  gap: 8px;
}

.vs-qa .menu {
  flex: none;
  width: max-content;
  min-width: 247px;
  border: 1px solid var(--popup-border);
  background: var(--popup-bg);
  font-family: 'Segoe UI', -apple-system, sans-serif;
  font-size: 12px;
  color: var(--ui-text);
}

.vs-qa .menu-item {
  display: flex;
  justify-content: space-between;
  align-items: center;
  gap: 24px;
  height: 26px;
  padding: 0 10px;
  border-bottom: 1px solid var(--separator);
}

.vs-qa .menu-item:last-child {
  border-bottom: none;
}

.vs-qa .menu-item.selected {
  outline: 1px solid var(--ui-text);
  outline-offset: -1px;
  background: var(--selection-bg);
}

.vs-qa .chevron {
  font-size: 13px;
}

.vs-qa .preview {
  flex: none;
  padding: 12px 14px;
  border: 1px solid var(--popup-border);
  background: #202021;
}

.vs-qa .code {
  font-size: 13.5px;
}

.vs-qa .added {
  background: var(--added-bg);
}

.vs-qa .plus {
  position: absolute;
  left: 0;
  color: var(--added-plus);
}

.vs-qa .link {
  margin-top: 18px;
  font-family: 'Segoe UI', -apple-system, sans-serif;
  font-size: 12px;
  color: var(--link);
}
</style>
