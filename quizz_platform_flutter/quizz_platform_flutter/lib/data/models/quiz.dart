class Quiz {
  final int? id;
  final String title;
  final String description;

  const Quiz({
    this.id,
    required this.title,
    required this.description,
  });

  factory Quiz.fromJson(Map<String, dynamic> json) => Quiz(
        id: json['id'],
        title: json['title'] ?? '',
        description: json['description'] ?? '',
      );

  Map<String, dynamic> toJson() => {
        if (id != null) 'id': id,
        'title': title,
        'description': description,
      };
}