import http from "k6/http";

export const options = {
  stages: [
    { duration: "10s", target: 10 },
    { duration: "10s", target: 20 },
    { duration: "10s", target: 0 },
  ],
  thresholds: { http_req_duration: ["avg<100", "p(95)<200"] },
  noConnectionReuse: true,
  userAgent: "MyK6UserAgentString/1.0",
};

export default function () {
  http.get("https://hackathon.bz.it");
}
