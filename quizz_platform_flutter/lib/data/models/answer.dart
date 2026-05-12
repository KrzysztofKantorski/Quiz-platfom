class Answer {
  final int? id;
  String text;
  bool isCorrect;

  Answer({
    this.id,
    required this.text,
    this.isCorrect = false,
  });

  // Tworzy obiekt z mapy (z API)
  factory Answer.fromJson(Map<String, dynamic> json) {
    return Answer(
      id: json['id'],
      text: json['text'] ?? '',
      isCorrect: json['isCorrect'] ?? false,
    );
  }

  // Zamienia obiekt na mapę (do API)
  Map<String, dynamic> toJson() {
    return {
      if (id != null) 'id': id,
      'text': text,
      'isCorrect': isCorrect,
    };
  }
}