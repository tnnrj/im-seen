export function getChartData(queryId: string) {
  // axios.get(process.env.VUE_APP_API_URL + 'ChartData', queryId);
  return [queryId, queryId]
}