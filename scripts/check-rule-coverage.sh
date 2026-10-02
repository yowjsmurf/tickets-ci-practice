#!/usr/bin/env bash
# Every rule ID in specs/*.md (lines like "- TICKETS-LIST-R01 WHEN ...") must have
# at least one test whose name contains "[TICKETS-LIST-R01]". Exit 1 if any is missing.
set -uo pipefail

SPECS_DIR="${SPECS_DIR:-specs}"
TESTS_DIR="${TESTS_DIR:-tests}"
missing=0
total=0
rows=""

while IFS=: read -r file line id; do
  [ -z "$id" ] && continue
  total=$((total + 1))
  if grep -rqF --include='*.cs' --include='*.razor' "[$id]" "$TESTS_DIR"; then
    rows+="| \`$id\` | ✅ may test |"$'\n'
    echo "OK       $id"
  else
    rows+="| \`$id\` | ❌ walang test |"$'\n'
    echo "MISSING  $id  ($file line $line)"
    echo "::error file=$file,line=$line::Rule $id is in the page doc but has no test. Add a test named \"[$id] ...\"."
    missing=1
  fi
done < <(grep -HnoE '^- [A-Z]+(-[A-Z]+)*-R[0-9]+' "$SPECS_DIR"/*.md | sed -E 's/^([^:]+):([0-9]+):- /\1:\2:/')

if [ "$total" -eq 0 ]; then
  echo "::warning::No rule IDs found in $SPECS_DIR/*.md"
fi

if [ -n "${GITHUB_STEP_SUMMARY:-}" ]; then
  {
    echo "### Rule coverage"
    echo ""
    echo "| Rule | Status |"
    echo "|---|---|"
    printf "%s" "$rows"
  } >> "$GITHUB_STEP_SUMMARY"
fi

exit $missing
