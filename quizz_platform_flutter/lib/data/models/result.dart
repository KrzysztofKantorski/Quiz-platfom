class QuizResult {
  final int totalPoints;
  final int maxPoints;
  final Duration timeTaken;
  final Map<int, int?> selectedAnswers; // questionIndex : answerIndex

  QuizResult({
    required this.totalPoints,
    required this.maxPoints,
    required this.timeTaken,
    required this.selectedAnswers,
  });

  double get percentage => maxPoints > 0 ? (totalPoints / maxPoints) * 100 : 0;
}