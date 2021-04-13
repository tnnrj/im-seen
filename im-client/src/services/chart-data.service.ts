export function getChartData(queryId: string) {
  // axios.get(process.env.API_URL + 'ChartData', queryId);
  return [queryId, queryId]
}